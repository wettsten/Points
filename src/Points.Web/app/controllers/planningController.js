'use strict';
app.controller('planningController', ['$scope', 'authService', 'catsService', 'planningTasksService', function ($scope, authService, catsService, planningTasksService) {

    $scope.tasks = [];
    $scope.cats = [];
    $scope.alerts = [];
    $scope.filteredCats = [];
    $scope.taskInEdit = { id: '' };

    $scope.loadCats = function () {
        catsService.getCatsByUser(authService.authentication.userId).then(function (results) {
            $scope.cats = results.data;
            for (var i = 0; i < $scope.cats.length; i++) {
                $scope.cats[i].tasks = [];
                $scope.cats[i].isOpen = true;
            }
            $scope.loadTasks();
        }, function (error) {
            $scope.message = 'Error loading data';
        });
    };

    $scope.loadTasks = function () {
        planningTasksService.getTasksByUser(authService.authentication.userId).then(function (results) {
            $scope.tasks = results.data;
            for (var i = 0; i < $scope.tasks.length; i++) {
                $scope.lookupCategory($scope.tasks[i]);
            }
            $scope.filterCats();
        }, function (error) {
            $scope.message = 'Error loading data';
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
            if ($scope.cats[i].id === task.categoryId) {
                $scope.cats[i].tasks.push(task);
                break;
            }
        }
    };

    $scope.addAlert = function (type, msg) {
        $scope.alerts.push({ type: type, msg: msg });
    };

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };

    $scope.loadCats();
}]);