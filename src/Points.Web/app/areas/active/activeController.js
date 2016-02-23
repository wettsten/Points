'use strict';
app.controller('activeController', ['$scope', 'resourceService', function ($scope, resourceService) {

    $scope.tasks = [];

    var loadTasks = function () {
        resourceService.get('activetasks');
    };

    resourceService.subscribe('activetasks', function (data) {
        $scope.tasks = data;
    });

    loadTasks();
}]);