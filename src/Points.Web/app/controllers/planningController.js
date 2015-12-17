'use strict';
app.controller('planningController', ['$scope', 'catsService', 'planningTasksService', '$timeout', function ($scope, catsService, planningTasksService, $timeout) {

    $scope.tasks = [];
    $scope.cats = [];
    $scope.alerts = [];
    $scope.filteredCats = [];
    $scope.taskInEdit = { id: '' };

    $scope.loadCats = function () {
        catsService.getCats().then(function (results) {
            $scope.cats = results.data;
            for (var i = 0; i < $scope.cats.length; i++) {
                $scope.cats[i].tasks = [];
                $scope.cats[i].isOpen = true;
            }
            $scope.loadTasks();
        }, function (err) {
            $scope.addAlert('danger', err.statusText);
        });
    };

    $scope.loadTasks = function () {
        planningTasksService.getTasks().then(function (results) {
            $scope.tasks = results.data;
            for (var i = 0; i < $scope.tasks.length; i++) {
                $scope.lookupCategory($scope.tasks[i]);
            }
            $scope.filterCats();
        }, function (err) {
            $scope.addAlert('danger', err.statusText);
        });
    };

    $scope.filterCats = function() {
        $scope.filteredCats = $scope.cats.filter(
        function (cat) {
            return cat.tasks.length > 0;
        });
    };

    $scope.lookupCategory = function (task) {
        for (var i = 0; i < $scope.cats.length; i++) {
            if ($scope.cats[i].id === task.task.category.id) {
                $scope.cats[i].tasks.push(task);
                break;
            }
        }
    };

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