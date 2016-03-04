﻿(function () {
    'use strict';

    angular
        .module('checkpoint')
        .controller('activeController', activeController);

    activeController.$inject = ['$scope', 'resourceService', 'filterService', '$timeout'];

    function activeController($scope, resourceService, filterService, $timeout) {
        /* jshint validthis:true */
        var activeVm = this;

        activeVm.noItems = false;
        activeVm.tasks = [];
        activeVm.taskFilter = filterService.getPTaskFilter();

        activate();

        function activate() {
            filterService.subscribe($scope, 'ataskFilter', getActiveTaskFilter);
            resourceService.get('activetasks', getActiveTasks);
        }

        function getActiveTaskFilter() {
            activeVm.taskFilter = filterService.getATaskFilter();
        }

        function getActiveTasks(data) {
            activeVm.tasks = data;
            if (data.length === 0) {
                activeVm.addWarning('No active tasks found');
                activeVm.noItems = true;
            }
        }
    }
})();
