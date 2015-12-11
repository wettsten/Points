'use strict';
app.directive('editTask', function () {
    return {
        scope: {
            task: '=theTask',
            cats: '='
        },
        templateUrl: '/app/views/directives/editTask.html',
        replace: true,
        controller: 'editTaskController'
    };
}).controller('editTaskController', [
    '$scope', 'tasksService', 'catsService', 'authService', function($scope, tasksService, catsService, authService) {

   $scope.editTask = {};

    $scope.isInEditMode = function (taskId) {
        return $scope.$parent.$parent.editTaskId === taskId;
    };

    $scope.isSomeoneElseInEditMode = function (taskId) {
        return $scope.$parent.$parent.editTaskId !== '' && $scope.$parent.$parent.editTaskId !== taskId;
    };

    $scope.clearEditData = function () {
        $scope.editTask = {};
        $scope.$parent.$parent.editTaskId = '';
        $scope.editForm.$hide();
    };

    $scope.saveEdit = function () {
        $scope.editForm.$submit();
        if ($scope.editForm.$dirty) {
            $scope.editForm.$show();
            return;
        }
        $scope.editTask.categoryId = $scope.editTask.category.id;
        $scope.editTask.userId = authService.authentication.userId;
        tasksService.editTask($scope.editTask).then(function (response) {
                $scope.clearEditData();
            $scope.$parent.loadTasks();
        },
         function (err) {
             $scope.$parent.$parent.$parent.message = err.data.message;
             $scope.editForm.$show();
         });
    };

    $scope.startEdit = function (taskId) {
        for (var i = 0; i < $scope.$parent.tasks.length; i++) {
            if ($scope.$parent.tasks[i].id === taskId) {
                $scope.editTask = angular.copy($scope.$parent.tasks[i]);
                $scope.$parent.$parent.editTaskId = taskId;
                $scope.editForm.$show();
                break;
            }
        }
    };

    $scope.deleteTask = function (taskId) {
        tasksService.deleteTask(taskId).then(function (response) {
            $scope.$parent.loadTasks();
        },
         function (err) {
             $scope.$parent.$parent.$parent.message = err.data.message;
         });
    };

    $scope.validateName = function (data) {
        if (!data) {
            $scope.editForm.$setDirty();
            return "Name is required!";
        }
        $scope.editForm.$setPristine();
    };
}]);