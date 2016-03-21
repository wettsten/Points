﻿(function() {
    'use strict';

    angular
        .module('checkpoint')
        .directive('activeTaskTotals', activeTaskTotals);
    
    function activeTaskTotals() {
        var directive = {
            restrict: 'EA',
            scope: {},
            templateUrl: '/app/areas/active/activeTaskTotals.html',
            controller: 'activeTaskTotalsController',
            controllerAs: 'aTotalsVm',
            bindToController: true
        };
        return directive;
    }

    angular
        .module('checkpoint')
        .controller('activeTaskTotalsController', activeTaskTotalsController);

    activeTaskTotalsController.$inject = ['resourceService'];

    function activeTaskTotalsController(resourceService) {
        /* jshint validthis:true */
        var aTotalsVm = this;

        aTotalsVm.isSuccess = false;
        aTotalsVm.totals = {};
        aTotalsVm.user = {};
        aTotalsVm.hideCats = true;
        aTotalsVm.totalClass = 'active';
        aTotalsVm.toggleCats = toggleCats;
        aTotalsVm.toggleTasks = toggleTasks;

        activate();

        function activate() {
            resourceService.get('activetasks/totals', getActiveTotals);
            resourceService.get('users', getUsers);
        }

        function getActiveTotals(data) {
            aTotalsVm.totals = data;
            for (var cat in aTotalsVm.totals.categories) {
                aTotalsVm.totals.categories[cat].hideTasks = true;
                calculateItemClass(aTotalsVm.totals.categories[cat]);
                for (var task in aTotalsVm.totals.categories[cat].tasks) {
                    calculateItemClass(aTotalsVm.totals.categories[cat].tasks[task]);
                }
            }
            calculateTotalClass();
        }

        function getUsers(data) {
            aTotalsVm.user = data[0];
            calculateTotalClass();
        }

        function calculateTotalClass () {
            if (typeof aTotalsVm.user.activeTargetPoints !== 'undefined' && typeof aTotalsVm.totals.totalPoints !== 'undefined') {
                var pct = aTotalsVm.totals.totalPoints * 100 / aTotalsVm.user.activeTargetPoints;
                if (pct >= 100) {
                    aTotalsVm.totalClass = 'success';
                    aTotalsVm.isSuccess = true;
                } else if (pct >= 50) {
                    aTotalsVm.totalClass = 'warning';
                } else if (pct >= 0) {
                    aTotalsVm.totalClass = 'danger';
                } else {
                    aTotalsVm.totalClass = 'active';
                }
            }
        }

        function calculateItemClass (item) {
            var pct = item.totalPoints * 100 / item.targetPoints;
            if (pct >= 100) {
                item.class = 'success';
            } else if (pct >= 50) {
                item.class = 'warning';
            } else {
                item.class = 'danger';
            }
        }

        function toggleCats () {
            aTotalsVm.hideCats = !aTotalsVm.hideCats;
            for (var cat in aTotalsVm.totals.categories) {
                aTotalsVm.totals.categories[cat].hideTasks = true;
            }
        }

        function toggleTasks (cat) {
            cat.hideTasks = !cat.hideTasks;
        }
    }

})();