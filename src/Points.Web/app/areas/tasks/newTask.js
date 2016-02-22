'use strict';
app.directive('newTask', function () {
    return {
        scope: {
            addSuccess: '&',
            addError: '&'
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
        resourceService.get('categories');
    };

    resourceService.registerForUpdates('categories', function (data) {
        $scope.cats = data;
        $scope.clearAddData();
    });

    $scope.addTask = function () {
        $scope.addTaskData.categoryId = $scope.addTaskData.category.id;
        resourceService.add('tasks', $scope.addTaskData).then(
            function (response) {
                var name = $scope.addTaskData.name;
                $scope.clearAddData();
                $timeout(
                    function () {
                        $scope.addSuccess({ msg: "Task '" + name + "' successfully added" });
                    }, 100
                );
            },
            function (err) {
                $scope.addError({ msg: err.data.message });
            }
        );
    };

    loadCats();
}]);