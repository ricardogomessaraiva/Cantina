app.controller('cadastradosCtrl', function ($scope, $http, $filter) {
    angular.element('#cadastro-usuario').addClass("start active open");
    $scope.errors = [];
    $scope.baseParent = {};
    $scope.parent = {};
    $scope.showDetails = false;

    $scope.init = function () {
        $http.get('Cadastrados/Status')
            .then(function success(response) {
                $scope.statusList = angular.copy(response.data.status);
                response.data.status.unshift({ Id: 0, CreatedAt: new Date, Description: 'Todos' });
                $scope.baseStatus = response.data.status;
                $scope.baseParent.Status = $scope.baseStatus[0];
            }, function error(response) {
                $scope.errors.push(response.statusText);
            });

        $scope.search();
    }

    $scope.search = function () {
        $http.post('Cadastrados/Search', { parent: $scope.baseParent })
            .then(function success(response) {
                $scope.parentList = response.data.parentList;
                $scope.parentList.forEach(function (parent, pos) {
                    parent.CreatedAt = moment(parent.CreatedAt).format('LL');
                    parent.Status.CreatedAt = moment(parent.Status.CreatedAt).format('LL');

                    parent.Phone.forEach(function (phone, pos) {
                        phone.CreatedAt = moment(phone.CreatedAt).format('LL');
                    });

                    moment.locale();
                    parent.Students.forEach(function (student) {
                        student.CreatedAt = moment(student.CreatedAt).format('LL');
                        student.BirthDate = moment(student.BirthDate).format('LL');
                        student.Period.CreatedAt = moment(student.Period.CreatedAt).format('LL');
                    });
                });
            }, function error(response) {
                $scope.errors.push(response.statusText);
            });
    }

    $scope.update = function (parent) {        
        $http.post('Cadastrados/Update', { parent: parent })
            .then(function success(response) {

        }), function error(response) {
            $scope.errors.push(response.statusText);
        }
    };

    $scope.details = function (parent) {
        $scope.showDetails = !$scope.showDetails;
        if ($scope.showDetails) {
            $scope.parent = parent;
        }
    };
});