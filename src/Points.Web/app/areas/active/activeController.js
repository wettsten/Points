'use strict';
app.controller('activeController', ['$scope', 'resourceService', function ($scope, resourceService) {

    $scope.tasks = [];

    resourceService.get('activetasks', function (data) {
        $scope.tasks = data;
    });
}]);