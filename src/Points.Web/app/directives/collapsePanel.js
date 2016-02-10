'use strict';
app.directive('collapsePanel', function () {
    return {
        scope: {
            isOpen: '=',
            headerTitle: '='
        },
        templateUrl: '/app/views/directives/collapsePanel.html',
        transclude: true
    };
});