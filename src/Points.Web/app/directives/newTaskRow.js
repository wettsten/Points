﻿'use strict';
app.directive('newTaskRow', function () {
    return {
        scope: {},
        templateUrl: '/app/views/directives/newTaskRow.html',
        replace: true,
        controller: 'newTaskRowController'
    };
}).controller('newTaskRowController', [
    '$scope', 'tasksService', 'catsService', 'authService', function($scope, tasksService, catsService, authService) {
        
        $scope.addTaskData = {};
    $scope.cats = [];

    $scope.clearAddData = function () {
        $scope.addTaskData = {};
    };

    $scope.loadCats = function () {
        catsService.getCatsByUser(authService.authentication.userId).then(function (results) {
            $scope.cats = results.data;
            $scope.addTaskData.category = $scope.cats[0];
            $scope.addForm.$show();
        }, function (error) {
            $scope.$parent.message = 'Error loading data';
        });
    };

    $scope.addTask = function () {
        $scope.addForm.$submit();
        if ($scope.addForm.$dirty) {
            return;
        }
        $scope.addTaskData.categoryId = $scope.selectedAdd.cat.id;
        $scope.addTaskData.userId = authService.authentication.userId;
        tasksService.addTask($scope.addTaskData).then(function (response) {
            $scope.$parent.loadTasks();
                $scope.clearAddData();
            },
         function (err) {
             $scope.$parent.message = err.data.message;
             $scope.addForm.$show();
         });
    };

    $scope.validateName = function (data) {
        if (!data) {
            $scope.addForm.$setDirty();
            return "Name is required!";
        }
        $scope.addForm.$setPristine();
    };

    $scope.loadCats();
}]);