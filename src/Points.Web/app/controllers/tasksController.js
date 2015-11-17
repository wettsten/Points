'use strict';
app.controller('tasksController', [
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

        $scope.editTask = {
            id: ""
        };
        $scope.tasks = [];
        $scope.addTaskData = {
            id: "",
            name: "",
            userId: "",
            categoryId: "",
            isPrivate: false,
            duration: {
                type: "",
                value: 0,
                unit: ""
            },
            frequency: {
                type: "",
                value: 0,
                unit: ""
            }
        };
        $scope.message = "";

    $scope.enums = {
        dTypes: [],
        dUnits: [],
        fTypes: [],
        fUnits: []
    };

    $scope.getEnums = function() {
        tasksService.getEnums().then(function(results) {
            $scope.enums.dTypes = results.data.dTypes;
            $scope.enums.dUnits = results.data.dUnits;
            $scope.enums.fTypes = results.data.fTypes;
            $scope.enums.fUnits = results.data.fUnits;
        }, function(error) {
            //alert(error.data.message);
        });
    };

    $scope.hideData = function (taskId) {
        return $scope.editTask.id === taskId;
    };

    $scope.hideEditCancel = function () {
        return $scope.editTask.id.length > 0;
    };

    $scope.showSaveCancel = function (taskId) {
        return $scope.editTask.id === taskId;
    };

    $scope.clearData = function () {
        $scope.taskData = {
            id: "",
            name: ""
        };
        $scope.editTask = {
            id: ""
        };
    };

    $scope.loadTasks = function () {
        tasksService.getTasksByUser(authService.authentication.userId).then(function (results) {
            $scope.tasks = results.data;
        }, function (error) {
            //alert(error.data.message);
        });
    };

    $scope.addTask = function () {
        tasksService.addtask($scope.addTaskData).then(function (response) {
            $scope.loadTasks();
        },
         function (err) {
             if (err.status === 409) {
                 $scope.message = 'Task name already exists';
             } else {
                 $scope.message = err.status + ' ' + err.data;
             }
         });
    };

    $scope.saveEdit = function () {
        tasksService.editTask($scope.editTask).then(function (response) {
            $scope.loadTasks();
        },
         function (err) {
             if (err.status === 409) {
                 $scope.message = 'Category already exists';
             } else {
                 $scope.message = err.status + ' ' + err.data;
             }
         });
    };

    $scope.startEdit = function (taskId) {
        for (var i = 0; i < $scope.tasks.length; i++) {
            if ($scope.tasks[i].id === taskId) {
                $scope.editTask = {
                    id: $scope.tasks[i].id,
                    name: $scope.tasks[i].name,
                    category: {
                        id: $scope.tasks[i].category.id,
                        name: $scope.tasks[i].category.name
                    },
                    isPrivate: $scope.tasks[i].isPrivate,
                    duration: {
                        type: $scope.tasks[i].duration.type,
                        value: $scope.tasks[i].duration.value,
                        unit: $scope.tasks[i].duration.unit
                    },
                    frequency: {
                        type: $scope.tasks[i].frequency.type,
                        value: $scope.tasks[i].frequency.value,
                        unit: $scope.tasks[i].frequency.unit
                    }
                };
                break;
            }
        }
    };

    $scope.cancelEdit = function () {
        $scope.editTask = {
            id: ""
        };
    };

    $scope.deleteTask = function (taskId) {
        tasksService.deleteTask(taskId).then(function (response) {
            $scope.loadTasks();
        },
         function (err) {
             if (err.status === 409) {
                 $scope.message = 'Category already exists';
             } else {
                 $scope.message = err.status + ' ' + err.data;
             }
         });
    };

    $scope.loadTasks();
    $scope.getEnums();
}]);