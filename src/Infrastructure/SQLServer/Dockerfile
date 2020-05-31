FROM mcr.microsoft.com/mssql/server

RUN mkdir -p /usr/work

WORKDIR /usr/work

COPY Infrastructure/SQLServer/. /usr/work/

USER root
RUN chmod +x /usr/work/migrate.sh

EXPOSE 1433

CMD /bin/bash ./entrypoint.sh