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
}).controller('newTaskController', ['$scope', 'resourceService', '$timeout', function ($scope, resourceService, $timeout) {

    $scope.cats = [];
    $scope.addTaskData = {};

    $scope.clearAddData = function () {
        $scope.addTaskData = {
            category: $scope.cats[0]
        };
    };

    var loadCats = function() {
        $scope.cats = resourceService.get('categories');
    };

    resourceService.registerForUpdates('categories', function (data) {
        $scope.cats = data;
    });

    $scope.addTask = function () {
        $scope.addTaskData.categoryId = $scope.addTaskData.category.id;
        resourceService.add('tasks', $scope.addTaskData).then(
            function (response) {
                $scope.clearAddData();
                $timeout(
                    function () {
                        $scope.addAlert({ type: 'success', msg: 'Task successfully added' });
                    }, 100
                );
            },
            function (err) {
                $scope.addAlert({ type: 'danger', msg: err.data.message });
            }
        );
    };

    loadCats();
}]);