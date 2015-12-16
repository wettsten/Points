'use strict';
app.controller('tasksController', [
    '$scope', 'tasksService', 'catsService', 'authService', 'filterFactory', '$timeout', function ($scope, tasksService, catsService, authService, filterFactory, $timeout) {

    $scope.tasks = [];
    $scope.alerts = [];
    $scope.taskInEdit = { id: '' };
    $scope.taskFilter = filterFactory.getTaskFilter();

    $scope.loadCats = function () {
        catsService.getCatsByUser(authService.authentication.userId).then(function (results) {
            $scope.cats = results.data;
            $scope.loadTasks();
        }, function (err) {
            $scope.addAlert('danger', err.data.message);
        });
    };

    $scope.loadTasks = function () {
        tasksService.getTasksByUser(authService.authentication.userId).then(function (results) {
            $scope.tasks = results.data;
            for (var i = 0; i < $scope.tasks.length; i++) {
                $scope.lookupCategory($scope.tasks[i]);
            }
        }, function (err) {
            $scope.addAlert('danger', err.data.message);
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

    $scope.$on('refreshTasks', function () {
        $scope.loadCats();
    });

    $scope.addAlert = function (type, msg) {
        var alert = { type: type, msg: msg };
        $scope.alerts.push(alert);
        $timeout(function () {
            if ($scope.alerts.indexOf(alert) > -1) {
                $scope.alerts.splice($scope.alerts.indexOf(alert), 1);
            }
        }, 5000);
    };

    $scope.closeAlert = function (alert) {
        if ($scope.alerts.indexOf(alert) > -1) {
            $scope.alerts.splice($scope.alerts.indexOf(alert), 1);
        }
    };

    $scope.loadCats();
}]);