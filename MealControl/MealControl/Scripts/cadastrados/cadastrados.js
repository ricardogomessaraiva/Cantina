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

    $scope.update = function (parent, pos) {
        $http.post('Cadastrados/Update', { parent: parent })
            .then(function success(response) {
                if (response.data.errors.length > 0) {
                    response.data.errors.forEach(function (error, pos) {
                        $scope.errors.push(error.ErrorMessage);
                    });
                    return;
                }

                var _parent = response.data.parent;
                _parent.Phone.forEach(function (phone, pos) {
                    phone.CreatedAt = moment(phone.CreatedAt).format('LL');
                });
                _parent.Students.forEach(function (student, pos) {
                    student.BirthDate = moment(student.BirthDate).format('LL');
                });

                _parent.CreatedAt = moment(_parent.CreatedAt).format('LL');
                _parent.ModifiedAt = moment(_parent.ModifiedAt).format('LL');
                $scope.parentList[pos] = _parent;
                $scope.statusList = angular.copy($scope.statusList);
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