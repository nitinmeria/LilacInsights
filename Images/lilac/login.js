
//====================================================================================================
//	File Name		:	login.js
//----------------------------------------------------------------------------------------------------
//	Purpose			:	Client side validation in JavaScript.
//	Author			:	
//	Creation Date	:	09-Feb-2004
//	Copyright		:	Copyrights © 2004 Spacecom
//	Email			:	nirmal_909@yahoo.com
//	History			:
//						Date				Author					Remark
//						09-Feb-2004						Initial Release
//
//====================================================================================================


//====================================================================================================
//	Function Name	:	Form_Submit()
//	Purpose			:	This function will executed when user submits a form. It checks validity of 
//						every field in the form.
//	Parameters		:	frm  - form name
//	Return			:	true or false
//	Author			:	
//	Creation Date	:	09-Feb-2004
//----------------------------------------------------------------------------------------------------

//This function use for validate Admin login
function Form_Submit(frm)
{
	with(frm)
    	{
    		if(!IsEmpty(username, 'Please, Enter UserName.'))
			return false;
        	
    		if(!IsEmpty(password, 'Please, Enter Password.'))
			return false;
        
        return true;
    }
}


//====================================================================================================
//	Function Name	:	Form_Submit()
//	Purpose			:	This function will executed when user submits a form. It checks validity of 
//						every field in the form.
//	Parameters		:	frm  - form name
//	Return			:	true or false
//	Author			:	
//	Creation Date	:	09-Feb-2004
//----------------------------------------------------------------------------------------------------

//This function use for validate Admin login
function validatePass(frm)
{
	with(frm)
    {
    	if(!IsEmpty(oldpass, 'Favor De Ingresar Contraseño.'))
        {
			return false;
        }
    	if(!IsEmpty(newpass, 'Favor De Ingresar Contraseño Nuevo.'))
        {
			return false;
        }
    	if(!IsEmpty(repass, 'Favor De Ingresar Contraseño Nuevo Otra Vez.'))
        {
			return false;
        }

    	if(newpass.value != repass.value)
        {
			alert("Favor De Ingresar Nuevo Contraseño Igual");
			return false;
        }
    	
        return true;
    }
}

//====================================================================================================
//	Function Name	:	Delete()
//	Purpose			:	This function will executed when user submits a form. It checks validity of 
//						every field in the form.
//	Parameters		:	frm  - form name
//	Return			:	true or false
//	Author			:	
//	Creation Date	:	09-Feb-2004
//----------------------------------------------------------------------------------------------------

//This function use for validate Admin login
function Delete(userId)
{
	if(confirm("¿Esta Seguro Querer Eliminar this Usuario ? "))
		location.href = "users.php?action=delete&userId="+userId;
}
