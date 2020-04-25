# FROM nginx:latest

# COPY nginx.conf /etc/nginx/nginx.conf

FROM nginx:latest

# EXPOSE 443
EXPOSE 80

# COPY localhost.cert /etc/nginx
# COPY localhost.key /etc/nginx
COPY nginx.conf /etc/nginx

CMD ["nginx", "-g", "daemon off;"]