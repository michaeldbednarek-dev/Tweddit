# Check if pom.xml (Maven project) or build.gradle (Gradle project) exists in the current directory
if (Test-Path "pom.xml") {
    # Maven project detected, generate Dockerfile for Spring Boot using Maven
    $dockerfileContent = @"
# Use an OpenJDK image as a base image
FROM openjdk:19-jdk-slim

# Set the working directory in the container
WORKDIR /app

# Copy the Maven build artifact (JAR file) into the container
COPY target/*.jar app.jar

# Expose the port your Spring Boot application will run on (e.g., 8080)
EXPOSE 8080

# Command to run the Spring Boot application
CMD ["java", "-jar", "/app/app.jar"]
"@
    Set-Content -Path "Dockerfile" -Value $dockerfileContent
    Write-Host "Dockerfile for Spring Boot Maven application has been created."

} elseif (Test-Path "build.gradle") {
    # Gradle project detected, generate Dockerfile for Spring Boot using Gradle
    $dockerfileContent = @"
# Use an OpenJDK image as a base image
FROM openjdk:17-jdk-slim

# Set the working directory in the container
WORKDIR /app

# Copy the Gradle build artifact (JAR file) into the container
COPY build/libs/*.jar app.jar

# Expose the port your Spring Boot application will run on (e.g., 8080)
EXPOSE 8080

# Command to run the Spring Boot application
CMD ["java", "-jar", "/app/app.jar"]
"@
    Set-Content -Path "Dockerfile" -Value $dockerfileContent
    Write-Host "Dockerfile for Spring Boot Gradle application has been created."

} else {
    Write-Host "No pom.xml or build.gradle file found. This does not appear to be a Spring Boot project."
}
