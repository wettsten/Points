(function() {
    'use strict';

    angular
        .module('checkpoint')
        .directive('editTask', editTask);
    
    function editTask() {
        var directive = {
            restrict: 'EA',
            scope: {
                task: '=theTask',
                taskInEdit: '=',
                addSuccess: '&',
                addError: '&'
            },
            templateUrl: '/app/areas/tasks/editTask.html',
            controller: 'editTaskController',
            controllerAs: 'editTaskVm',
            bindToController: true
        };
        return directive;
    }

    angular
        .module('checkpoint')
        .controller('editTaskController', editTaskController);

    editTaskController.$inject = ['$scope', 'resourceService', 'modalService'];

    function editTaskController($scope, resourceService, modalService) {
        /* jshint validthis:true */
        var editTaskVm = this;

        editTaskVm.cats = [];
        editTaskVm.editTask = {};
        editTaskVm.isInEditMode = isInEditMode;
        editTaskVm.clearEditData = clearEditData;
        editTaskVm.startEdit = startEdit;
        editTaskVm.saveEdit = saveEdit;
        editTaskVm.delete = deleteTask;

        activate();

        function activate() { 
            $scope.$watch('taskInEdit.id', function () {
                if (editTaskVm.taskInEdit.id !== '' && editTaskVm.taskInEdit.id !== editTaskVm.task.id) {
                    editTaskVm.editTask = {};
                }
            });
            
            resourceService.get('categories', function (data) {
                editTaskVm.cats = data;
            });
        }

        function isInEditMode () {
            return editTaskVm.taskInEdit.id === editTaskVm.task.id;
        }

        function clearEditData () {
            editTaskVm.editTask = {};
            editTaskVm.taskInEdit.id = '';
        }

        function startEdit () {
            editTaskVm.editTask = angular.copy(editTaskVm.task);
            editTaskVm.taskInEdit.id = editTaskVm.task.id;
        }

        function saveEdit () {
            var name = editTaskVm.editTask.name;
            resourceService.edit('tasks', editTaskVm.editTask).then(
                function (response) {
                    clearEditData();
                    editTaskVm.addSuccess({ msg: "Task '{0}' successfully updated".format(name) });
                },
                function (err) {
                    editTaskVm.addError({ msg: err.data.message });
                }
            );
        }

        function deleteTask () {
            var name = editTaskVm.task.name;
            modalService.newModal('confirmDelete', 'common', { name: editTaskVm.task.name, id: editTaskVm.task.id }, 'sm',
                function (result) {
                    resourceService.remove('tasks', editTaskVm.task.id).then(
                        function (response) {
                            editTaskVm.addSuccess({ msg: "Task '{0}' successfully deleted".format(name) });
                        },
                        function (err) {
                            editTaskVm.addError({ msg: err.data.message });
                        }
                    );
                }
            );
        }
    }

})();