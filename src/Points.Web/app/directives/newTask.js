'use strict';
app.directive('newTask', function () {
    return {
        scope: {},
        templateUrl: '/app/views/directives/newTask.html',
        replace: true,
        controller: 'newTaskController'
    };
}).controller('newTaskController', [
    '$scope', 'tasksService', 'catsService', 'authService', function($scope, tasksService, catsService, authService) {
        
        $scope.addTaskData = {};
    $scope.cats = [];

    $scope.clearAddData = function () {
        $scope.addTaskData = {};
    };

    $scope.loadCats = function () {
        catsService.getCatsByUser(authService.authentication.userId).then(function (results) {
            $scope.cats = results.data;
            $scope.addTaskData.cat = $scope.cats[0];
        }, function (error) {
            $scope.$parent.message = 'Error loading data';
        });
    };

    $scope.addTask = function () {
        $scope.addTaskData.categoryId = $scope.selectedAdd.cat.id;
        $scope.addTaskData.userId = authService.authentication.userId;
        tasksService.addTask($scope.addTaskData).then(function (response) {
            $scope.$parent.loadTasks();
                $scope.clearAddData();
            },
         function (err) {
             $scope.$parent.message = err.data.message;
         });
    };

    $scope.loadCats();
}]);