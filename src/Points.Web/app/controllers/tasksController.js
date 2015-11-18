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
        
    $scope.selectedAdd = {};
    $scope.editTask = {
        id: ""
    };
    $scope.tasks = [];
    $scope.addTaskData = {};
    $scope.message = "";

    $scope.enums = {
        dTypes: [],
        dUnits: [],
        fTypes: [],
        fUnits: []
    };
    $scope.cats = [];

    $scope.getDurationValueAddMin = function () {
        if ($scope.selectedAdd.dType.id !== 'None' || $scope.selectedAdd.dType.id === '') {
            return 1;
        } else {
            return 0;
        }
    };

    $scope.getFrequencyValueAddMin = function () {
        if ($scope.selectedAdd.fType.id !== 'Once' || $scope.selectedAdd.fType.id === '') {
            return 1;
        } else {
            return 0;
        }
    };

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
            $scope.loadTasks();
        }, function(error) {
            //alert(error.data.message);
        });
    };

    $scope.showAddDuration = function() {
        return $scope.selectedAdd.dType.id !== 'None' || $scope.selectedAdd.dType.id === '';
    };

    $scope.showAddFrequency = function () {
        return $scope.selectedAdd.fType.id !== 'Once' || $scope.selectedAdd.fType.id === '';
    };

    $scope.hideEditData = function (taskId) {
        return $scope.editTask.id === taskId;
    };

    $scope.hideEditCancel = function () {
        return $scope.editTask.id.length > 0;
    };

    $scope.hideEditDuration = function() {
        return $scope.editTask.duration.dType.id !== 'None' || $scope.editTask.duration.dType.id === '';
    };

    $scope.hideEditFrequency = function () {
        return $scope.editTask.frequency.fType.id !== 'Once' || $scope.editTask.frequency.fType.id === '';
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
            $scope.getEnums();
        }, function (error) {
            //alert(error.data.message);
        });
    };

    $scope.lookupCategory = function(task) {
        for (var i = 0; i < $scope.cats.length; i++) {
            if ($scope.cats[i].id === task.categoryId) {
                task.category = $scope.cats[i];
                break;
            }
        }
    };

    $scope.lookupDType = function (task) {
        for (var i = 0; i < $scope.enums.dTypes.length; i++) {
            if ($scope.enums.dTypes[i].id === task.duration.type) {
                task.duration.dType = $scope.enums.dTypes[i];
                break;
            }
        }
    };

    $scope.lookupDUnit = function (task) {
        for (var i = 0; i < $scope.enums.dUnits.length; i++) {
            if ($scope.enums.dUnits[i].id === task.duration.unit) {
                task.duration.dUnit = $scope.enums.dUnits[i];
                break;
            }
        }
    };

    $scope.lookupFType = function (task) {
        for (var i = 0; i < $scope.enums.fTypes.length; i++) {
            if ($scope.enums.fTypes[i].id === task.frequency.type) {
                task.frequency.fType = $scope.enums.fTypes[i];
                break;
            }
        }
    };

    $scope.lookupFUnit = function (task) {
        for (var i = 0; i < $scope.enums.fUnits.length; i++) {
            if ($scope.enums.fUnits[i].id === task.frequency.unit) {
                task.frequency.fUnit = $scope.enums.fUnits[i];
                break;
            }
        }
    };

    $scope.loadTasks = function () {
        tasksService.getTasksByUser(authService.authentication.userId).then(function (results) {
            $scope.tasks = results.data;
            for (var i = 0; i < $scope.tasks.length; i++) {
                $scope.lookupCategory($scope.tasks[i]);
                $scope.lookupDType($scope.tasks[i]);
                $scope.lookupDUnit($scope.tasks[i]);
                $scope.lookupFType($scope.tasks[i]);
                $scope.lookupFUnit($scope.tasks[i]);
            }
            $scope.clearData();
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
        $scope.addTaskData.userId = authService.authentication.userId;
        tasksService.addTask($scope.addTaskData).then(function (response) {
            $scope.loadTasks();
        },
         function (err) {
             $scope.message = err.status + ' ' + err.data.message;
         });
    };

    $scope.saveEdit = function () {
        $scope.editTask.categoryId = $scope.editTask.category.id;
        $scope.editTask.duration.type = $scope.editTask.duration.dType.id;
        $scope.editTask.duration.unit = $scope.editTask.duration.dUnit.id;
        $scope.editTask.frequency.type = $scope.editTask.frequency.fType.id;
        $scope.editTask.frequency.unit = $scope.editTask.frequency.fUnit.id;
        $scope.editTask.userId = authService.authentication.userId;
        tasksService.editTask($scope.editTask).then(function (response) {
            $scope.loadTasks();
        },
         function (err) {
             $scope.message = err.status + ' ' + err.data.message;
         });
    };

    $scope.startEdit = function (taskId) {
        for (var i = 0; i < $scope.tasks.length; i++) {
            if ($scope.tasks[i].id === taskId) {
                $scope.editTask = {
                    id: $scope.tasks[i].id,
                    name: $scope.tasks[i].name,
                    category: $scope.tasks[i].category,
                    isPrivate: $scope.tasks[i].isPrivate,
                    duration: {
                        dType: $scope.tasks[i].duration.dType,
                        type: $scope.tasks[i].duration.type,
                        value: $scope.tasks[i].duration.value,
                        dUnit: $scope.tasks[i].duration.dUnit,
                        unit: $scope.tasks[i].duration.unit
                    },
                    frequency: {
                        fType: $scope.tasks[i].frequency.fType,
                        type: $scope.tasks[i].frequency.type,
                        value: $scope.tasks[i].frequency.value,
                        fUnit: $scope.tasks[i].frequency.fUnit,
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
             $scope.message = err.status + ' ' + err.data.message;
         });
    };

    $scope.loadCats();
}]);