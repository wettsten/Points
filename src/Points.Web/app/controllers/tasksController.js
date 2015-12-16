'use strict';
app.controller('tasksController', [
    '$scope', 'tasksService', 'catsService', 'authService', 'filterFactory', function ($scope, tasksService, catsService, authService, filterFactory) {

    $scope.tasks = [];
    $scope.alerts = [];
    $scope.taskInEdit = { id: '' };
    $scope.taskFilter = filterFactory.getTaskFilter();

    $scope.loadCats = function () {
        catsService.getCatsByUser(authService.authentication.userId).then(function (results) {
            $scope.cats = results.data;
            $scope.loadTasks();
        }, function (err) {
            $scope.addAlert({ type: 'danger', msg: err.data.message });
        });
    };

    $scope.loadTasks = function () {
        tasksService.getTasksByUser(authService.authentication.userId).then(function (results) {
            $scope.tasks = results.data;
            for (var i = 0; i < $scope.tasks.length; i++) {
                $scope.lookupCategory($scope.tasks[i]);
            }
        }, function (err) {
            $scope.addAlert({ type: 'danger', msg: err.data.message });
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

    $scope.addAlert = function (type, msg) {
        $scope.alerts.push({ type: type, msg: msg });
    };

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };

    $scope.loadCats();
}]);