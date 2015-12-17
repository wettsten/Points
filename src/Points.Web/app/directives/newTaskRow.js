'use strict';
app.directive('newTaskRow', function () {
    return {
        scope: {
            addAlert: '&'
        },
        templateUrl: '/app/views/directives/newTaskRow.html',
        replace: true,
        controller: 'newTaskRowController'
    };
}).controller('newTaskRowController', ['$scope', 'tasksService', 'catsService', '$timeout', function ($scope, tasksService, catsService, $timeout) {
        
    $scope.addTaskData = {};
    $scope.cats = [];

    $scope.clearAddData = function () {
        $scope.addTaskData = {
            category: $scope.cats[0]
        };
    };

    $scope.loadCats = function () {
        catsService.getCats().then(function (results) {
            $scope.cats = results.data;
            $scope.addTaskData.category = $scope.cats[0];
        }, function (err) {
            $scope.addAlert({ type: 'danger', msg: err.statusText });
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

    $scope.loadCats();
}]);