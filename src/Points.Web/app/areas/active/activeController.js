'use strict';
app.controller('activeController', ['$scope', 'resourceService', 'filterFactory', '$timeout', function ($scope, resourceService, filterFactory, $timeout) {

    $scope.noItems = false;
    $scope.tasks = [];
    $scope.taskFilter = filterFactory.getPTaskFilter();

    filterFactory.subscribe($scope, 'ptaskFilter', function () {
        $scope.taskFilter = filterFactory.getPTaskFilter();
    });
    
    $timeout(function () { 
        resourceService.get('activetasks', function (data) {
            $scope.tasks = data;
            if (data.length === 0) {
                if ($scope.addWarning) {
                    $scope.addWarning('No active tasks found');
                }
                $scope.noItems = true;
            }
        });
    }, 100);
}]);