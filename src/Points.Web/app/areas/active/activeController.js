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

    resourceService.subscribe('activetasks', function (data) {
        $scope.cats = data;
        if ($scope.cats.length === 0) {
            $scope.addWarning('No active tasks found');
        }
        setupCats();
    });

    $scope.check = function (task) {
        task.timesCompleted += 1;
        resourceService.edit('activetasks', task).then(
            function (response) {
                $scope.addSuccess('Task successfully checked');
            },
            function (err) {
                $scope.addError(err.data.message);
            }
        );
    };

    $scope.uncheck = function (task) {
        modalService.newModal('confirmUncheck', { name: task.name, id: task.id }, 'sm',
            function (result) {
                task.timesCompleted -= 1;
                resourceService.edit('activetasks', task).then(
                    function (response) {
                        $scope.addSuccess('Task successfully unchecked');
                    },
                    function (err) {
                        $scope.addError(err.data.message);
                    }
                );
            }
        );
    };

    loadCats();
}]);