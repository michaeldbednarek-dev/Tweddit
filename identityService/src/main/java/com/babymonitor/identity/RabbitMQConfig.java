package com.babymonitor.identity;
import org.springframework.amqp.core.Queue;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class RabbitMQConfig {

    public static final String DELETED_USER_QUEUE = "deletedUser";

    @Bean
    public Queue deletedUserQueue() {
        return new Queue(DELETED_USER_QUEUE, true); // Durable queue
    }
}
