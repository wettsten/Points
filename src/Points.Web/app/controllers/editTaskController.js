'use strict';
app.controller('editTaskController', [
    '$scope', 'tasksService', 'catsService', 'authService', 'ngAuthSettings', function($scope, tasksService, catsService, authService, ngAuthSettings) {

    $scope.setDeleteIcon = function(task, isActive) {
        task.deleteIcon = isActive ? ngAuthSettings.icons.deleteActiveIcon : ngAuthSettings.icons.deleteIcon;
    };

    $scope.setEditIcon = function(task, isActive) {
        task.editIcon = isActive ? ngAuthSettings.icons.editActiveIcon : ngAuthSettings.icons.editIcon;
    };

    $scope.setCancelIcon = function(task, isActive) {
        task.cancelIcon = isActive ? ngAuthSettings.icons.cancelActiveIcon : ngAuthSettings.icons.cancelIcon;
    };

    $scope.setSaveIcon = function(task, isActive) {
        task.saveIcon = isActive ? ngAuthSettings.icons.saveActiveIcon : ngAuthSettings.icons.saveIcon;
    };
        
    $scope.editTask = {};

    $scope.isInEditMode = function () {
        return $scope.editTask.id ? true : false;
    };

    $scope.amIInEditMode = function (taskId) {
        return $scope.editTask.id === taskId;
    };

    $scope.hideEditDuration = function () {
        return $scope.editTask.duration.dType.id !== 'None' || $scope.editTask.duration.dType.id === '';
    };

    $scope.hideEditFrequency = function () {
        return $scope.editTask.frequency.fType.id !== 'Once' || $scope.editTask.frequency.fType.id === '';
    };

    $scope.ignoreDurationValueAndUnit = function(taskId) {
        if ($scope.amIInEditMode(taskId)) {
            return $scope.editTask.duration.dType.id === 'None';
        } else {
            for (var i = 0; i < $scope.tasks.length; i++) {
                if ($scope.tasks[i].id === taskId) {
                    return $scope.tasks[i].duration.type === 'None';
                }
            }
        }
    };

    $scope.ignoreFrequencyValueAndUnit = function (taskId) {
        if ($scope.amIInEditMode(taskId)) {
            return $scope.editTask.frequency.fType.id === 'Once';
        } else {
            for (var i = 0; i < $scope.tasks.length; i++) {
                if ($scope.tasks[i].id === taskId) {
                    return $scope.tasks[i].frequency.type === 'Once';
                }
            }
        }
    };

    $scope.showSaveCancel = function (taskId) {
        return $scope.editTask.id === taskId;
    };

    $scope.clearEditData = function () {
        $scope.editTask = {};
    };

    $scope.saveEdit = function () {
        $scope.editTask.categoryId = $scope.editTask.category.id;
        $scope.editTask.duration.type = $scope.editTask.duration.dType.id;
        $scope.editTask.duration.unit = $scope.editTask.duration.dUnit.id;
        $scope.editTask.frequency.type = $scope.editTask.frequency.fType.id;
        $scope.editTask.frequency.unit = $scope.editTask.frequency.fUnit.id;
        $scope.editTask.userId = authService.authentication.userId;
        tasksService.editTask($scope.editTask).then(function (response) {
                $scope.clearEditData();
            $scope.loadTasks();
        },
         function (err) {
             $scope.message = err.data.message;
         });
    };

    $scope.startEdit = function (taskId) {
        for (var i = 0; i < $scope.tasks.length; i++) {
            if ($scope.tasks[i].id === taskId) {
                $scope.editTask = angular.copy($scope.tasks[i]);
                break;
            }
        }
    };

    $scope.deleteTask = function (taskId) {
        tasksService.deleteTask(taskId).then(function (response) {
            $scope.loadTasks();
        },
         function (err) {
             $scope.message = err.data.message;
         });
    };
}]);