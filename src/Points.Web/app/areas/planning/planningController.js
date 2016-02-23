'use strict';
app.controller('planningController', ['$scope', 'resourceService', '$timeout', 'modalService', function ($scope, resourceService, $timeout, modalService) {

    $scope.tasks = [];
    $scope.availableTasks = false;
    $scope.alerts = [];
    $scope.taskInEdit = { id: '' };

    var setupCats = function() {
        for (var i = 0; i < $scope.tasks.length; i++) {
            $scope.tasks[i].isOpen = i === 0;
        }
    };

    var loadCats = function () {
        resourceService.get('planningtasks');
        resourceService.get('availabletasks');
    };

    resourceService.subscribe('planningtasks', function (data) {
        $scope.tasks = data;
        if ($scope.tasks.length === 0) {
            $scope.addWarning('No planning tasks found');
        }
        setupCats();
    });

    resourceService.subscribe('availabletasks', function (data) {
        $scope.availableTasks = data.length > 0;
    });

    $scope.addTask = function () {
        modalService.newModal('newPlanningTask', null, 'lg',
            function (result) {
                $scope.addSuccess("Task '{0}' successfully added".format(result.name));
            }
        );
    };

    loadCats();
}]);