'use strict';
app.directive('activeTaskCard', function () {
    return {
        scope: {
            task: '=theTask',
            addSuccess: '&',
            addError: '&'
        },
        templateUrl: '/app/views/directives/activeTaskCard.html',
        replace: true,
        controller: 'activeTaskCardController'
    };
}).controller('activeTaskCardController', ['$scope', 'resourceService', 'modalService', function ($scope, resourceService, modalService) {

    $scope.status = function () {
        var pct = $scope.task.timesCompleted * 100 / $scope.task.frequency.value;
        if ($scope.task.frequency.type.id === 'AtMost') {
            if (pct > 100) {
                return 'panel-danger';
            } else if (pct > 50) {
                return 'panel-warning';
            }
            return 'panel-success';
        }
        if (pct >= 100) {
            return 'panel-success';
        } else if (pct >= 50) {
            return 'panel-warning';
        }
        return 'panel-danger';
    };

    $scope.check = function (task) {
        task.timesCompleted += 1;
        resourceService.edit('activetasks', task).then(
            function (response) {
                $scope.addSuccess({ msg: 'Task successfully checked' });
            },
            function (err) {
                $scope.addError({ msg: err.data.message });
            }
        );
    };

    $scope.uncheck = function (task) {
        modalService.newModal('confirmUncheck', { name: task.name, id: task.id }, 'sm',
            function (result) {
                task.timesCompleted -= 1;
                resourceService.edit('activetasks', task).then(
                    function (response) {
                        $scope.addSuccess({ msg: 'Task successfully unchecked' });
                    },
                    function (err) {
                        $scope.addError({ msg: err.data.message });
                    }
                );
            }
        );
    };
}]);