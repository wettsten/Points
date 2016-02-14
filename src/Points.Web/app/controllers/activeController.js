'use strict';
app.controller('activeController', ['$scope', 'resourceService', '$timeout', '$uibModal', function ($scope, resourceService, $timeout, $uibModal) {

    $scope.tasks = [];
    $scope.cats = [];
    $scope.alerts = [];
    $scope.filteredCats = [];
    $scope.taskInEdit = { id: '' };

    var setupCats = function () {
        for (var i = 0; i < $scope.cats.length; i++) {
            $scope.cats[i].isOpen = i === 0;
        }
    };

    var loadCats = function () {
        $scope.cats = resourceService.get('activetasks');
        setupCats();
        $timeout(function () {
            if ($scope.cats.length === 0) {
                $scope.addAlert('warning', 'No active tasks found');
            }
        }, 1000);
    };

    resourceService.registerForUpdates('activetasks', function (data) {
        $scope.cats = data;
        setupCats();
    });

    $scope.addAlert = function (type, msg) {
        var alert = { type: type, msg: msg };
        $scope.alerts.push(alert);
        $timeout(function () {
            if ($scope.alerts.indexOf(alert) > -1) {
                $scope.alerts.splice($scope.alerts.indexOf(alert), 1);
            }
        }, 5000);
    };

    $scope.closeAlert = function (alert) {
        if ($scope.alerts.indexOf(alert) > -1) {
            $scope.alerts.splice($scope.alerts.indexOf(alert), 1);
        }
    };

    $scope.check = function (task) {
        task.timesCompleted += 1;
        resourceService.edit('activetasks', task).then(
            function (response) {
                $scope.addAlert('success', 'Task successfully checked');
            },
            function (err) {
                $scope.addAlert('danger', err.data.message);
            }
        );
    };

    $scope.uncheck = function (task) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/app/views/partials/confirmUncheck.html',
            controller: 'confirmUncheckController',
            size: 'sm',
            resolve: {
                item: function () {
                    return {
                        name: task.name,
                        id: task.id
                    };
                }
            }
        });

        modalInstance.result.then(function (result) {
            if (result !== 'cancel') {
                task.timesCompleted -= 1;
                resourceService.edit('activetasks', task).then(
                    function (response) {
                        $scope.addAlert('success', 'Task successfully unchecked');
                    },
                    function (err) {
                        $scope.addAlert('danger', err.data.message);
                    }
                );
            }
        });
    };

    loadCats();
}]);