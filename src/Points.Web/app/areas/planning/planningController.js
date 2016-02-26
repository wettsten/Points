'use strict';
app.controller('planningController', ['$scope', 'resourceService', '$timeout', 'modalService', 'filterFactory', function ($scope, resourceService, $timeout, modalService, filterFactory) {

    $scope.tasks = [];
    $scope.availableTasks = false;
    $scope.taskFilter = filterFactory.getPTaskFilter();

    filterFactory.subscribe($scope, 'ptaskFilter', function () {
        $scope.taskFilter = filterFactory.getPTaskFilter();
    });

    var setupCats = function() {
        for (var i = 0; i < $scope.tasks.length; i++) {
            $scope.tasks[i].isOpen = i === 0;
        }
    };

    var loadCats = function () {
        resourceService.get('planningtasks', function (data) {
            $scope.tasks = data;
            if ($scope.tasks.length === 0) {
                $scope.addWarning('No planning tasks found');
            }
            setupCats();
        });
        resourceService.get('availabletasks', function (data) {
            $scope.availableTasks = data.length > 0;
        });
    };

    $scope.addTask = function () {
        modalService.newModal('newPlanningTask', null, 'lg',
            function (result) {
                $scope.addSuccess("Task '{0}' successfully added".format(result.name));
            }
        );
    };

    loadCats();
}]);