'use strict';
app.directive('basicPanel', function () {
    return {
        scope: {
            headerTitle: '='
        },
        templateUrl: '/app/views/directives/basicPanel.html',
        transclude: true
    };
});