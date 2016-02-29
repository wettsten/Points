'use strict';
app.controller('activeController', ['$scope', 'resourceService', 'filterFactory', function ($scope, resourceService, filterFactory) {

    $scope.noItems = false;
    $scope.tasks = [];
    $scope.taskFilter = filterFactory.getPTaskFilter();

    filterFactory.subscribe($scope, 'ptaskFilter', function () {
        $scope.taskFilter = filterFactory.getPTaskFilter();
    });

    resourceService.get('activetasks', function (data) {
        $scope.tasks = data;
        if (data.length === 0) {
            $scope.addWarning('No active tasks found');
            $scope.noItems = true;
        }
    });
}]);