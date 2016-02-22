'use strict';
app.directive('editTask', function () {
    return {
        scope: {
            task: '=theTask',
            taskInEdit: '=',
            addSuccess: '&',
            addError: '&'
        },
        templateUrl: '/app/views/directives/editTask.html',
        replace: true,
        controller: 'editTaskController'
    };
}).controller('editTaskController', ['$scope', 'resourceService', 'modalService', function ($scope, resourceService, modalService) {

    $scope.cats = [];
    $scope.editTask = {};

    $scope.isInEditMode = function () {
        return $scope.taskInEdit.id === $scope.task.id;
    };

    $scope.$watch('taskInEdit.id', function () {
        if ($scope.taskInEdit.id !== '' && $scope.taskInEdit.id !== $scope.task.id) {
            $scope.editTask = {};
        }
    });

    $scope.clearEditData = function () {
        $scope.editTask = {};
        $scope.taskInEdit.id = '';
    };

    var loadCats = function () {
        resourceService.get('categories');
    };

    resourceService.registerForUpdates('categories', function (data) {
        $scope.cats = data;
    });

    $scope.startEdit = function () {
        $scope.editTask = angular.copy($scope.task);
        $scope.taskInEdit.id = $scope.task.id;
    };

    $scope.saveEdit = function () {
        var name = $scope.editTask.name;
        resourceService.edit('tasks',$scope.editTask).then(
            function (response) {
                $scope.clearEditData();
                $scope.addSuccess({ msg: "Task '{0}' successfully updated".format(name) });
            },
            function (err) {
                $scope.addError({ msg: err.data.message });
            }
        );
    };

    $scope.delete = function () {
        var name = $scope.task.name;
        modalService.newModal('confirmDelete', { name: $scope.task.name, id: $scope.task.id }, 'sm',
            function (result) {
                resourceService.delete('tasks',$scope.task.id).then(
                    function (response) {
                        $scope.addSuccess({ msg: "Task '{0}' successfully deleted".format(name) });
                    },
                    function (err) {
                        $scope.addError({ msg: err.data.message });
                    }
                );
            }
        );
    };

    loadCats();
}]);