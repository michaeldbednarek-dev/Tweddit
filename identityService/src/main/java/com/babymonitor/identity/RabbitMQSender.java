package com.babymonitor.identity;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class RabbitMQSender {

    @Autowired
    private RabbitTemplate rabbitTemplate;

    public void sendDeletedUserMessage(String message) {
        rabbitTemplate.convertAndSend(RabbitMQConfig.DELETED_USER_QUEUE, message);
    }
}