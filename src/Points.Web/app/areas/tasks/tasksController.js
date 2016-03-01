'use strict';
app.controller('tasksController', ['$scope', 'resourceService', 'filterFactory', '$timeout', function ($scope, resourceService, filterFactory, $timeout) {

    $scope.noItems = false;
    $scope.tasks = [];
    $scope.taskInEdit = { id: '' };
    $scope.taskFilter = filterFactory.getTaskFilter();

    filterFactory.subscribe($scope, 'taskFilter', function () {
        $scope.taskFilter = filterFactory.getTaskFilter();
    });

    $timeout(function() {
        resourceService.get('tasks', function(data) {
            $scope.tasks = data;
            if (data.length === 0) {
                $scope.addWarning('No tasks found');
                resourceService.get('categories', function(data2) {
                    if (data2.length === 0) {
                        $scope.noItems = true;
                    }
                });
            }
        });
    }, 100);
}]);