(function () {
    'use strict';

    angular
        .module('checkpoint')
        .controller('activeController', activeController);

    activeController.$inject = ['$scope', 'resourceService', 'filterFactory', '$timeout'];

    function activeController($scope, resourceService, filterFactory, $timeout) {
        /* jshint validthis:true */
        var activeVm = this;

        activeVm.noItems = false;
        activeVm.tasks = [];
        activeVm.taskFilter = filterFactory.getPTaskFilter();

        activate();

        function activate() {
            filterFactory.subscribe($scope, 'ptaskFilter', function () {
                activeVm.taskFilter = filterFactory.getPTaskFilter();
            });

            $timeout(function () {
                resourceService.get('activetasks', function (data) {
                    activeVm.tasks = data;
                    if (data.length === 0) {
                        if (activeVm.addWarning) {
                            activeVm.addWarning('No active tasks found');
                        }
                        activeVm.noItems = true;
                    }
                });
            }, 100);
        }
    }
})();
