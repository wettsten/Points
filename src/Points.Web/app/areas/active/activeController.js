'use strict';
app.controller('activeController', ['$scope', 'resourceService', '$timeout', 'modalService', function ($scope, resourceService, $timeout, modalService) {

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
        resourceService.get('activetasks');
    };

    resourceService.registerForUpdates('activetasks', function (data) {
        $scope.cats = data;
        if ($scope.cats.length === 0) {
            $scope.addAlert('warning', 'No active tasks found');
        }
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
        modalService.newModal('confirmUncheck', { name: task.name, id: task.id }, 'sm',
            function (result) {
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
        );
    };

    loadCats();
}]);