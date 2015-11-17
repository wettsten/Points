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
        
    $scope.selectedAdd = {
        cat: {
            id: "",
            name: ""
        },
        dType: {
            id: "",
            name: ""
        },
        dUnit: {
            id: "",
            name: ""
        },
        fType: {
            id: "",
            name: ""
        },
        fUnit: {
            id: "",
            name: ""
        }
    };
    $scope.editTask = {
        id: ""
    };
    $scope.tasks = [];
    $scope.addTaskData = {
        id: "",
        name: "",
        userId: "",
        categoryId: $scope.selectedAdd.cat.id,
        isPrivate: false,
        duration: {
            type: $scope.selectedAdd.dType.id,
            value: 0,
            unit: $scope.selectedAdd.dUnit.id
        },
        frequency: {
            type: $scope.selectedAdd.fType.id,
            value: 0,
            unit: $scope.selectedAdd.fUnit.id
        }
    };
    $scope.message = "";

    $scope.enums = {
        dTypes: [],
        dUnits: [],
        fTypes: [],
        fUnits: []
    };
    $scope.cats = [];

    $scope.getEnums = function() {
        tasksService.getEnums().then(function(results) {
            $scope.enums.dTypes = results.data.dTypes;
            $scope.enums.dUnits = results.data.dUnits;
            $scope.enums.fTypes = results.data.fTypes;
            $scope.enums.fUnits = results.data.fUnits;
            $scope.selectedAdd.dType = $scope.enums.dTypes[0];
            $scope.selectedAdd.dUnit = $scope.enums.dUnits[0];
            $scope.selectedAdd.fType = $scope.enums.fTypes[0];
            $scope.selectedAdd.fUnit = $scope.enums.fUnits[0];
        }, function(error) {
            //alert(error.data.message);
        });
    };

    $scope.showAddDuration = function() {
        return $scope.selectedAdd.dType.id !== 'None';
    };

    $scope.showAddFrequency = function () {
        return $scope.selectedAdd.fType.id !== 'Once';
    };

    $scope.hideEditData = function (taskId) {
        return $scope.editTask.id === taskId;
    };

    $scope.hideEditCancel = function () {
        return $scope.editTask.id.length > 0;
    };

    $scope.showSaveCancel = function (taskId) {
        return $scope.editTask.id === taskId;
    };

    $scope.clearData = function () {
        $scope.addTaskData = {
            id: "",
            name: "",
            userId: "",
            categoryId: $scope.selectedAdd.cat.id,
            isPrivate: false,
            duration: {
                type: $scope.selectedAdd.dType.id,
                value: 0,
                unit: $scope.selectedAdd.dUnit.id
            },
            frequency: {
                type: $scope.selectedAdd.fType.id,
                value: 0,
                unit: $scope.selectedAdd.fUnit.id
            }
        };
        $scope.editTask = {
            id: ""
        };
    };

    $scope.loadCats = function () {
        catsService.getCats().then(function (results) {
            $scope.cats = results.data;
            $scope.selectedAdd.cat = $scope.cats[0];
        }, function (error) {
            //alert(error.data.message);
        });
    };

    $scope.loadTasks = function () {
        tasksService.getTasksByUser(authService.authentication.userId).then(function (results) {
            $scope.tasks = results.data;
        }, function (error) {
            //alert(error.data.message);
        });
    };

    $scope.addTask = function () {
        $scope.addTaskData.categoryId = $scope.selectedAdd.cat.id;
        $scope.addTaskData.duration.type = $scope.selectedAdd.dType.id;
        $scope.addTaskData.duration.unit = $scope.selectedAdd.dUnit.id;
        $scope.addTaskData.frequency.type = $scope.selectedAdd.fType.id;
        $scope.addTaskData.frequency.unit = $scope.selectedAdd.fUnit.id;
        tasksService.addTask($scope.addTaskData).then(function (response) {
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

    $scope.loadCats();
    $scope.getEnums();
    $scope.loadTasks();
}]);