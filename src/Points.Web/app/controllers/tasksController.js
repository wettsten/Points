'use strict';
app.controller('tasksController', [
    '$scope', 'tasksService', 'catsService', 'authService', 'filterFactory', function ($scope, tasksService, catsService, authService, filterFactory) {

    $scope.tasks = [];
    $scope.message = '';
    $scope.editTaskId = '';
    $scope.taskFilter = filterFactory.getTaskFilter();

    $scope.loadCats = function () {
        catsService.getCatsByUser(authService.authentication.userId).then(function (results) {
            $scope.cats = results.data;
            $scope.loadTasks();
        }, function (error) {
            $scope.message = 'Error loading data';
        });
    };

    $scope.loadTasks = function () {
        tasksService.getTasksByUser(authService.authentication.userId).then(function (results) {
            $scope.tasks = results.data;
            for (var i = 0; i < $scope.tasks.length; i++) {
                $scope.lookupCategory($scope.tasks[i]);
            }
        }, function (error) {
            $scope.message = 'Error loading data';
        });
    };

    $scope.lookupCategory = function(task) {
        for (var i = 0; i < $scope.cats.length; i++) {
            if ($scope.cats[i].id === task.categoryId) {
                task.category = $scope.cats[i];
                break;
            }
        }
    };

    filterFactory.subscribe($scope, 'taskFilter', function taskFilterChanged() {
        $scope.taskFilter = filterFactory.getTaskFilter();
    });

    $scope.loadCats();
}]);