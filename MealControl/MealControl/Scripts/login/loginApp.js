var app = angular.module('loginApp', []);

app.controller('loginCtrl', function ($scope, $http) {
    $scope.loading = false;
    $scope.user = {};
    $scope.login = function () {
        $scope.response = null;
        if ($scope.formLogin.$invalid) {
            $scope.formLogin.$setDirty();
            $scope.response = { code: 412, text: 'Preencha usuário e senha corretamente.' };
            return;
        }

        $scope.loading = true;

        $http({
            method: 'POST',
            url: 'Login/Validate',
            data: { user: $scope.user }
        }).then(function successCallback(response) {
            window.location.href = 'Home';
        }, function errorCallback(response) {            
            $scope.response = { code: response.status, text: response.statusText };

            $scope.formLogin.$setPristine();
            $scope.loading = false;
        });
    }
});