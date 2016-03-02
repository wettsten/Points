(function() {
    'use strict';

    angular
        .module('checkpoint')
        .directive('planningTaskTotals', planningTaskTotals);
    
    function planningTaskTotals() {
        var directive = {
            restrict: 'EA',
            scope: {},
            templateUrl: '/app/areas/planning/planningTaskTotals.html',
            controller: 'planningTaskTotalsController',
            controllerAs: 'pTotalsVm'
        };
        return directive;
    }

    angular
        .module('checkpoint')
        .controller('planningTaskTotalsController', planningTaskTotalsController);

    planningTaskTotalsController.$inject = ['resourceService'];

    function planningTaskTotalsController(resourceService) {
        /* jshint validthis:true */
        var pTotalsVm = this;

        pTotalsVm.totals = {};
        pTotalsVm.user = {};
        pTotalsVm.hideCats = true;
        pTotalsVm.totalClass = 'active';
        pTotalsVm.toggleCats = toggleCats;
        pTotalsVm.toggleTasks = toggleTasks;

        activate();

        function activate() {
            resourceService.get('planningtotals', function (data) {
                pTotalsVm.totals = data;
                angular.forEach(pTotalsVm.totals.categories, function (cat) {
                    cat.hideTasks = true;
                    cat.filter = false;
                    angular.forEach(cat.tasks, function (task) {
                        task.filter = false;
                    });
                });
                calculateTotalClass();
            });
            resourceService.get('users', function (data) {
                pTotalsVm.user = data[0];
                calculateTotalClass();
            });
        }

        function calculateTotalClass () {
            if (pTotalsVm.user.targetPoints && pTotalsVm.totals.points) {
                var pct = pTotalsVm.totals.points * 100 / pTotalsVm.user.targetPoints;
                if (pct >= 100) {
                    pTotalsVm.totalClass = 'success';
                } else if (pct >= 50) {
                    pTotalsVm.totalClass = 'warning';
                } else if (pct > 0) {
                    pTotalsVm.totalClass = 'danger';
                } else {
                    pTotalsVm.totalClass = 'active';
                }
            }
        }

        function toggleCats () {
            pTotalsVm.hideCats = !pTotalsVm.hideCats;
            angular.forEach(pTotalsVm.totals.categories, function (cat) {
                cat.hideTasks = true;
            });
        }

        function toggleTasks (cat) {
            cat.hideTasks = !cat.hideTasks;
        }
    }

})();