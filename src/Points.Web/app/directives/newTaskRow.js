'use strict';
app.directive('newTaskRow', function () {
    return {
        scope: {},
        templateUrl: '/app/views/directives/newTaskRow.html',
        replace: true,
        controller: 'newTaskRowController'
    };
}).controller('newTaskRowController', ['$scope', 'tasksService', 'catsService', 'authService', '$timeout', function ($scope, tasksService, catsService, authService, $timeout) {
        
    $scope.addTaskData = {};
    $scope.cats = [];

    $scope.clearAddData = function () {
        $scope.addTaskData = {
            category: $scope.cats[0]
        };
    };

    $scope.loadCats = function () {
        catsService.getCatsByUser(authService.authentication.userId).then(function (results) {
            $scope.cats = results.data;
            $scope.addTaskData.category = $scope.cats[0];
        }, function (error) {
            $scope.$parent.message = 'Error loading data';
        });
    };

    $scope.addTask = function () {
        $scope.addTaskData.categoryId = $scope.addTaskData.category.id;
        $scope.addTaskData.userId = authService.authentication.userId;
        tasksService.addTask($scope.addTaskData).then(function (response) {
            $scope.clearAddData();
            $timeout(function () {
                $scope.$parent.loadTasks();
            }, 100);
            },
         function (err) {
             $scope.$parent.message = err.data.message;
         });
    };

    $scope.loadCats();
}]);