app.controller('cadastroCtrl', function ($scope, $http) {
    angular.element('#cadastro-usuario').addClass("start active open");
    $scope.phones = [{ Number: null }];
    $scope.students = [{ Name: null, BirthDate: null, Period: null }];
    $scope.retypeEmail = null;

    $scope.load = function () {
        $http.get('Cadastro/Load').then(success);
        function success(response) {
            $scope.periods = response.data.period;            
        };
    }

    $scope.save = function () {
        $scope.errors = [];
        $scope.Parent.Phone = $scope.phones;
        $scope.Parent.Students = $scope.students

        $scope.phones.forEach(function (phone, i) {
            if (phone.Number == null)
                $scope.Parent.Phone.splice(i, 1);
        });       
        
        return;
        $http.post('Cadastro/Save', { parent: $scope.Parent }).then(success, error);
        function success(response) {
            if (response.status == 201)//CREATED
                return window.location.href = 'Confirmacao';
            if (response.status == 202)
                alert('Atualizado com sucesso');

            for (var i in response.data) {
                $scope.errors.push(response.data[i].ErrorMessage);
            }
        };
        function error(response) {
            $scope.errors = new Array(response.statusText);
        };

    };

    $scope.addPhone = function () {
        $scope.phones.push({ Number: '' });
    }

    $scope.removePhone = function (phone) {
        var pos = $scope.phones.indexOf(phone);
        $scope.phones.splice(pos, 1);
    }

    $scope.addStudent = function () {
        $scope.students.push({
            Name: '',
            BirthDate: '',
            Period: ''
        });
    }

    $scope.removeStudent = function (i) {
        $scope.students.splice(i, 1);
    }

});