'use strict';
app.controller('tasksController', ['$scope', 'tasksService', 'authService', function ($scope, tasksService, authService) {

    $scope.tasks = [];

    tasksService.getTasksByUser(authService.authentication.userId).then(function (results) {
        
        $scope.tasks = results.data;

    }, function (error) {
        //alert(error.data.message);
    });

}]);