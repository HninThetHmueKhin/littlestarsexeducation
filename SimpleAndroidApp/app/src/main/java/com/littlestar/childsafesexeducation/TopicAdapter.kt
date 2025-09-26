package com.littlestar.childsafesexeducation

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView

class TopicAdapter(
    private val topics: List<Topic>,
    private val onTopicClick: (Topic) -> Unit
) : RecyclerView.Adapter<TopicAdapter.TopicViewHolder>() {

    class TopicViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        val topicIcon: TextView = itemView.findViewById(R.id.topicIcon)
        val topicTitle: TextView = itemView.findViewById(R.id.topicTitle)
        val topicDescription: TextView = itemView.findViewById(R.id.topicDescription)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): TopicViewHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.item_topic, parent, false)
        return TopicViewHolder(view)
    }

    override fun onBindViewHolder(holder: TopicViewHolder, position: Int) {
        val topic = topics[position]
        holder.topicIcon.text = topic.icon
        holder.topicTitle.text = topic.title
        holder.topicDescription.text = topic.description
        
        holder.itemView.setOnClickListener {
            onTopicClick(topic)
        }
    }

    override fun getItemCount(): Int = topics.size
}
