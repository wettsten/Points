(function() {
    'use strict';

    angular
        .module('checkpoint')
        .directive('newTask', newTask);
    
    function newTask() {
        var directive = {
            restrict: 'EA', 
            scope: {
                addSuccess: '&',
                addError: '&'
            },
            templateUrl: '/app/areas/tasks/newTask.html',
            controller: 'newTaskController',
            controllerAs: 'newTaskVm',
            bindToController: true
        };
        return directive;
    }

    angular
        .module('checkpoint')
        .controller('newTaskController', newTaskController);

    newTaskController.$inject = ['resourceService'];

    function newTaskController(resourceService) {
        /* jshint validthis:true */
        var newTaskVm = this;

        newTaskVm.cats = [];
        newTaskVm.addTaskData = {};
        newTaskVm.clearAddData = clearAddData;
        newTaskVm.addTask = addTask;

        activate();

        function activate() {
            resourceService.get('categories', function (data) {
                newTaskVm.cats = data;
                clearAddData();
            });
        }

        function clearAddData () {
            newTaskVm.addTaskData = {
                category: newTaskVm.cats[0]
            };
        }

        function addTask () {
            resourceService.add('tasks', newTaskVm.addTaskData).then(
                function (response) {
                    var name = newTaskVm.addTaskData.name;
                    clearAddData();
                    newTaskVm.addSuccess({ msg: "Task '{0}' successfully added".format(name) });
                },
                function (err) {
                    newTaskVm.addError({ msg: err.data.message });
                }
            );
        }
    }

})();