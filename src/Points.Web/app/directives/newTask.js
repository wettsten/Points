'use strict';
app.directive('newTask', function () {
    return {
        scope: {
            addAlert: '&'
        },
        templateUrl: '/app/views/directives/newTask.html',
        replace: true,
        controller: 'newTaskController'
    };
}).controller('newTaskController', ['$scope', 'tasksService', 'catsService', '$timeout', '$q', function ($scope, tasksService, catsService, $timeout, $q) {

    $scope.addTaskData = {};
    $scope.cats = [];

    $scope.clearAddData = function () {
        $scope.addTaskData = {
            category: $scope.cats[0]
        };
    };

    $scope.loadCats = catsService.getCats().then(
        function (results) {
            return results.data;
        });

    $scope.loadData = function () {
        $q.all([$scope.loadCats]).then(
            function (data) {
                $scope.cats = data[0];
                $scope.addTaskData.category = $scope.cats[0];
            });
    };

    $scope.addTask = function () {
        $scope.addTaskData.categoryId = $scope.addTaskData.category.id;
        tasksService.addTask($scope.addTaskData).then(function (response) {
            $scope.clearAddData();
            $timeout(function () {
                $scope.$emit('refreshTasks');
                $scope.addAlert({ type: 'success', msg: 'Task successfully added' });
            }, 100);
            },
         function (err) {
             $scope.addAlert({ type: 'danger', msg: err.data.message });
         });
    };

    $scope.loadData();
}]);