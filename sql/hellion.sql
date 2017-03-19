CREATE DATABASE  IF NOT EXISTS `hellion` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `hellion`;
-- MySQL dump 10.13  Distrib 5.7.12, for Win64 (x86_64)
--
-- Host: localhost    Database: hellion
-- ------------------------------------------------------
-- Server version	5.7.17-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `characters`
--

DROP TABLE IF EXISTS `characters`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `characters` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `accountId` int(11) NOT NULL,
  `angle` float NOT NULL,
  `bankCode` int(11) NOT NULL,
  `classId` int(11) NOT NULL,
  `dexterity` int(11) NOT NULL,
  `exp` bigint(20) NOT NULL,
  `faceId` int(11) NOT NULL,
  `fp` int(11) NOT NULL,
  `gender` tinyint(3) unsigned NOT NULL,
  `gold` int(11) NOT NULL,
  `hairColor` int(10) unsigned NOT NULL,
  `hairId` int(11) NOT NULL,
  `hp` int(11) NOT NULL,
  `intelligence` int(11) NOT NULL,
  `level` int(11) NOT NULL,
  `mapId` int(11) NOT NULL,
  `mp` int(11) NOT NULL,
  `name` longtext,
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  `skinSetId` int(11) NOT NULL,
  `slot` int(11) NOT NULL,
  `stamina` int(11) NOT NULL,
  `strength` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `items`
--

DROP TABLE IF EXISTS `items`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `items` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `characterId` int(11) NOT NULL,
  `itemCreatorId` int(11) NOT NULL,
  `element` tinyint(3) unsigned NOT NULL,
  `elementRefine` tinyint(3) unsigned NOT NULL,
  `itemCount` int(11) NOT NULL,
  `itemId` int(11) NOT NULL,
  `itemSlot` int(11) NOT NULL,
  `refine` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_items_characterId` (`characterId`),
  CONSTRAINT `FK_items_characters_characterId` FOREIGN KEY (`characterId`) REFERENCES `characters` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `users` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `authority` int(11) NOT NULL,
  `password` longtext,
  `username` longtext,
  `verification` bit(1) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2017-03-13 21:42:02
