'use strict';
app.controller('activeController', ['$scope', 'resourceService', 'filterFactory', function ($scope, resourceService, filterFactory) {

    $scope.tasks = [];
    $scope.taskFilter = filterFactory.getPTaskFilter();

    filterFactory.subscribe($scope, 'ptaskFilter', function () {
        $scope.taskFilter = filterFactory.getPTaskFilter();
    });

    resourceService.get('activetasks', function (data) {
        $scope.tasks = data;
    });
}]);