'use strict';
app.directive('socialLogin', function() {
    return {
        scope: {
        },
        templateUrl: '/app/views/partials/socialLogin.html',
        replace: true
    };
});