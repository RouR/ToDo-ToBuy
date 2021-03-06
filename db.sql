CREATE USER acc_srv_login WITH
	LOGIN
	NOSUPERUSER
	NOCREATEDB
	NOCREATEROLE
	CONNECTION LIMIT -1
	PASSWORD 'devOnlyPwd';

CREATE DATABASE acc_srv_db OWNER acc_srv_login;
REVOKE connect ON DATABASE acc_srv_db FROM PUBLIC;
GRANT connect ON DATABASE acc_srv_db TO acc_srv_login;
GRANT connect ON DATABASE acc_srv_db TO postgres;

CREATE USER todo_srv_login WITH
	LOGIN
	NOSUPERUSER
	NOCREATEDB
	NOCREATEROLE
	CONNECTION LIMIT -1
	PASSWORD 'devOnlyPwd';

CREATE DATABASE todo_srv_db OWNER todo_srv_login;
REVOKE connect ON DATABASE todo_srv_db FROM PUBLIC;
GRANT connect ON DATABASE todo_srv_db TO todo_srv_login;
GRANT connect ON DATABASE todo_srv_db TO postgres;



CREATE USER tobuy_srv_login WITH
	LOGIN
	NOSUPERUSER
	NOCREATEDB
	NOCREATEROLE
	CONNECTION LIMIT -1
	PASSWORD 'devOnlyPwd';

CREATE DATABASE tobuy_srv_db OWNER tobuy_srv_login;
REVOKE connect ON DATABASE tobuy_srv_db FROM PUBLIC;
GRANT connect ON DATABASE tobuy_srv_db TO tobuy_srv_login;
GRANT connect ON DATABASE tobuy_srv_db TO postgres;

