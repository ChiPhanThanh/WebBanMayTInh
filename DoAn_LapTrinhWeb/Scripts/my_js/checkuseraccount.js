//sử dụng "sweetalert2" để hiện các thông báo toast, thư viện được đã đuọc thêm vào main_layout  " <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11.0.19/dist/sweetalert2.all.min.js"></script>"
//kiểm tra đăng ký

function SaveForm() {
    var username = $("#Username").val();
    var email = $("#Email").val();
    var phone = $("#Phone").val();
    var password = $("#Password").val();
    var name = $("#Name").val();
    var address = $("#Addres_1").val();
    //check null họ tên
    if (name == "") {
        const Toast = Swal.mixin({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 2000,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })
        Toast.fire({
            icon: 'warning',
            title: 'Vui lòng nhập họ tên'
        })
        return false;
    }
  
    //check null username
    if (username == "") {
        const Toast = Swal.mixin({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 2000,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })
        Toast.fire({
            icon: 'warning',
            title: 'Vui lòng nhập Tên đăng nhập'
        })
        return false;
    }

    //check null email
    if (email == "") {
        const Toast = Swal.mixin({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 2000,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })
        Toast.fire({
            icon: 'warning',
            title: 'Vui lòng nhập Email'
        })
        return false;
    }
    //check null password
    if (password == "") {
        const Toast = Swal.mixin({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 2000,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })

        Toast.fire({
            icon: 'warning',
            title: 'Vui lòng nhập password'
        })
        return false;
    }

    //check null phone
    if (phone == "") {
        const Toast = Swal.mixin({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 2000,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })
        Toast.fire({
            icon: 'warning',
            title: 'Vui lòng nhập số điện thoại'
        })
        return false;
    }
    //check null address
    if (address == "") {
        const Toast = Swal.mixin({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 2000,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })
        Toast.fire({
            icon: 'warning',
            title: 'Vui lòng nhập địa chỉ'
        })
        return false;
    }
   
 
    // 
    var data = $("#Registration").serialize(); //truyền id "Registration" vô tag form của view Register.cshml
    $.ajax({
        type: "post",
        data: data,
        url: "/Account/SaveData", //check điều hiện ở action SaveData của controller "AccountController"
        success: function (result) {
            if (result == "existemail") { //check email đã có trong database chưa (xem check ràng buộc ở AccountController)

                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top-end',
                    showConfirmButton: false,
                    timer: 2000,
                    timerProgressBar: true,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })

                Toast.fire({
                    icon: 'error',
                    title: 'Email đã được sử dụng' 
                })
            } else
                if (result == "existusername") {
                    const Toast = Swal.mixin({
                        toast: true,
                        position: 'top-end',
                        showConfirmButton: false,
                        timer: 2000,
                        timerProgressBar: true,
                        didOpen: (toast) => {
                            toast.addEventListener('mouseenter', Swal.stopTimer)
                            toast.addEventListener('mouseleave', Swal.resumeTimer)
                        }
                    })

                    Toast.fire({
                        icon: 'error',
                        title: 'Tên đăng nhập đã tồn tại'
                    })

                } else if (result == "existphone") {//check phone đã có trong database chưa
                    const Toast = Swal.mixin({
                        toast: true,
                        position: 'top-end',
                        showConfirmButton: false,
                        timer: 2000,
                        timerProgressBar: true,
                        didOpen: (toast) => {
                            toast.addEventListener('mouseenter', Swal.stopTimer)
                            toast.addEventListener('mouseleave', Swal.resumeTimer)
                        }
                    })

                    Toast.fire({
                        icon: 'error',
                        title: 'SĐT đã được sử dụng'
                    })

                } else { 
                    window.location.href = "/Home/Index";//nếu check email, phone, user name không có trong database, thì sẽ thêm dữ liệu vào database và quay trở về trang chủ
                    const Toast = Swal.mixin({
                        toast: true,
                        position: 'top-end',
                        showConfirmButton: false,
                        timer: 2000,
                        timerProgressBar: true,
                        didOpen: (toast) => {
                            toast.addEventListener('mouseenter', Swal.stopTimer)
                            toast.addEventListener('mouseleave', Swal.resumeTimer)
                        }
                    })

                    Toast.fire({
                        icon: 'success',
                        title: 'Đăng ký thành công'
                    })

                }
        }
    });
}
//kiểm tra đăng nhập
var SignIn = function () {
    var username = $("#User").val();
    var password = $("#Pass").val();
    if (username == "") {
        const Toast = Swal.mixin({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 2000,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })


        Toast.fire({
            icon: 'warning',
            title: 'Vui lòng nhập Tên đăng nhập'
        })
        return false;
    }
    //check rỗng password
    if (password == "") {
        const Toast = Swal.mixin({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 2000,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })
        Toast.fire({
            icon: 'warning',
            title: 'Vui lòng nhập password'
        })
        return false;
    }


    var data = $("#loginForm").serialize();
    $.ajax({
        type: "POST",
        url: "/Account/CheckSignIn", //kiểm tra tồn tại username, username và password đã trùng chưa (kiểm tra ở acition checksignin của cotroller Account)
        data: data,
        success: function (result) {
            if (result == "Success") {
                $.toast({
                    heading: 'Thông báo',
                    text: 'Đăng nhập thành công',
                    icon: 'success',
                    hideAfter: 2000,
                    loader: false
                });
                window.location.href = "/Home/Index";
            } else {
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top-end',
                    showConfirmButton: false,
                    timer: 2000,
                    timerProgressBar: true,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })
                Toast.fire({
                    icon: 'error',
                    title: 'Sai mật khẩu hoặc tài khoản, hoặc tài khoản đã bị vô hiệu hóa'
                })
                $("#loginForm")[0].reset();
            }
        }
    })
}
//check ràng buộc quên mật khẩu
function ForgotPass() {
    var email = $("#Emailforgot").val();
    if (email == "") {
        const Toast = Swal.mixin({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 2000,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })
        Toast.fire({
            icon: 'warning',
            title: 'Vui lòng nhập Email'
        })
        return false;
    }

}

//hiện popup form modal đăng nhập
function PopupModalSignIn() {
    $("#ShowModalSignIn").modal();
}
//chỉnh sửa thông tin cá nhân
function SaveForm1(ev) {
    ev.preventDefault();        // Chặn sự kiện gửi mặc định của form
    var name = $("#Name").val();
    var phone = $("#Phone").val();
    var Addres_1 = $("#Addres_1").val();
    if (name == "") {
        $.toast({
            heading: 'Cành báo',
            text: 'Vui lòng nhập họ tên',
            icon: 'warning',
            hideAfter: 2000,
            loader: false
        });
        return false;
    }
    if (phone == "") {
        $.toast({
            heading: 'Cành báo',
            text: 'Vui lòng nhập số điện thoại',
            icon: 'warning',
            hideAfter: 2000,
            loader: false
        });
        return false;
    }
    if (Addres_1 == "") {
        $.toast({
            heading: 'Cành báo',
            text: 'Vui lòng nhập địa chỉ 1',
            icon: 'warning',
            hideAfter: 2000,
            loader: false
        });
        return false;
    }
    var data = $("#Updatename").serialize();
    $.ajax({
        type: "post",
        data: data,
        url: "/Account/Editprofile",
        success: function (result) {
             if (result == "Success") {
                $.toast({
                    heading: 'Thông báo',
                    text: 'Cập nhật dữ liệu người dùng thành công',
                    icon: 'success',
                    hideAfter: 5000,
                    loader: false
                });
            }
            else {
                $.toast({
                    heading: 'Lỗi',
                    text: 'Đã xảy ra lỗi không xác định',
                    icon: 'error',
                    hideAfter: 5000,
                    loader: false
                });
            }
        }
    });
}