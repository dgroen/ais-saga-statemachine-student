

wget https://dlcdn.apache.org/kafka/3.9.0/kafka_2.13-3.9.0.tgz

$ tar -xzf kafka_2.13-3.9.0.tgz
$ cd kafka_2.13-3.9.0

docker pull apache/kafka:3.9.0
docker run -p 9092:9092 apache/kafka:3.9.0

sudo apt install openjdk-8-jre

bin/kafka-console-producer.sh --topic quickstart-events --bootstrap-server localhost:9092

 bin/kafka-console-consumer.sh --topic quickstart-events --from-beginning --bootstrap-server localhost:9092